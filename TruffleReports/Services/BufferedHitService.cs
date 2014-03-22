using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MongoDB.Driver;
using TruffleReports.Contracts;

namespace TruffleReports.Services
{
    /// <summary>
    /// An implementation of <see cref="IHitService"/> that buffers <see cref="Hit"/>s until an internal count is reached before batch importing.
    /// </summary>
    internal class BufferedHitService : IHitService
    {
        private const string HIT_COLLECTION = "truffle_reports_hits";

        private readonly Subject<Hit> logged;
        private readonly MongoCollection<Hit> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedHitService" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="defaultDatabase">The default database.</param>
        /// <param name="buffer">The buffer.</param>
        public BufferedHitService(string connectionString, string defaultDatabase = "local", int buffer = 1000)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetServer().GetDatabase(defaultDatabase);
            collection = db.GetCollection<Hit>(HIT_COLLECTION);

            this.logged = new Subject<Hit>();
            this.logged.Buffer(buffer).Subscribe(Output);
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
            collection.InsertBatch(hits);
        }
    }
}
