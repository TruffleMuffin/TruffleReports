using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using TruffleReports.Contracts;
using TruffleReports.Helpers;

namespace TruffleReports.Services
{
    /// <summary>
    /// An implementation of <see cref="IHitService"/> that buffers <see cref="Hit"/>s until a count or
    /// maximum time duration is reached before batch importing.
    /// </summary>
    public class HitService : IHitService
    {
        private readonly Subject<Hit> logged;
        private readonly Subject<DateTime> loggedAt;
        private readonly MongoCollection<Hit> collection;
        private readonly ReportService service;

        /// <summary>
        /// Initializes the <see cref="HitService"/> class.
        /// </summary>
        static HitService()
        {
            // Always register this class correctly.
            if (BsonClassMap.IsClassMapRegistered(typeof(Hit)) == false)
            {
                BsonClassMap.RegisterClassMap<Hit>(cm =>
                {
                    cm.AutoMap();
                    cm.IdMemberMap.SetIdGenerator(GuidGenerator.Instance);
                });
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HitService" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="helper">The helper.</param>
        /// <param name="buffer">The buffer.</param>
        public HitService(ReportService service, RepositoryHelper helper, int buffer = 1000)
        {
            this.collection = helper.Database.GetCollection<Hit>(Consts.HIT_COLLECTION);

            this.service = service;
            
            this.logged = new Subject<Hit>();
            this.loggedAt = new Subject<DateTime>();

            this.logged.Buffer(TimeSpan.FromMinutes(1), buffer).Subscribe(Output);
            this.loggedAt.Buffer(6).Subscribe(Process);
        }

        /// <summary>
        /// Logs the specified <see cref="Hit" />.
        /// </summary>
        /// <param name="hit">The hit.</param>
        public void Log(Hit hit)
        {
            logged.OnNext(hit);
        }

        /// <summary>
        /// Outputs the specified <see cref="Hit"/>s to the persistance storage.
        /// </summary>
        /// <param name="hits">The hits.</param>
        private void Output(IList<Hit> hits)
        {
            // When there are hits
            if (hits.Count > 0)
            {
                // Import them
                collection.InsertBatch(hits);

                // Push the earliest and latest hit
                var order = hits.OrderBy(a => a.Logged).ToArray();
                loggedAt.OnNext(order.FirstOrDefault().Logged);
                loggedAt.OnNext(order.LastOrDefault().Logged);
            }
        }

        /// <summary>
        /// Kicks of the generation of updated reports.
        /// </summary>
        /// <param name="loggedAts">The logged ats.</param>
        private void Process(IList<DateTime> loggedAts)
        {
            if (loggedAts.Count > 0)
            {
                var order = loggedAts.OrderBy(a => a).ToArray();
                Task.Factory.StartNew(() => service.Generate(order.FirstOrDefault(), order.LastOrDefault()), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }
    }
}
