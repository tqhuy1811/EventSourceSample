using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace EventSource.Boilerplate.EventStore
{
	public class GetEventStore : IEventStore
	{
		private readonly IEventStoreConnection _esConn;
		private const string EventClrTypeHeader = "EventClrTypeName";

		public GetEventStore(IEventStoreConnection esConn)
		{
			_esConn = esConn;
		}

		public async Task CreateNewStreamAsync(string streamName,
			List<IDomainEvent> events)
		{
			await AppendEventsToStreamAsync(streamName, events);
		}

		public async Task AppendEventsToStreamAsync(
			string streamName,
			List<IDomainEvent> events,
			int? expectedVersion = null)
		{
			var commitId = Guid.NewGuid();

			var eventsInStorageFormat = events.Select(e =>
				MapToEventStoreStorageFormat(e, commitId, e.Id));

			await _esConn.AppendToStreamAsync(
				stream: StreamName(streamName),
				expectedVersion ?? ExpectedVersion.Any,
				eventsInStorageFormat
			);
		}

		private string StreamName(string streamName)
		{
			// Event Store projections require only a single hypen ("-")
			var sp = streamName.Split(new[] {'-'}, 2);
			return sp[0] + "-" + sp[1].Replace("-", "");
		}

		private EventData MapToEventStoreStorageFormat(
			object domainEvent,
			Guid commitId,
			Guid eventId)
		{
			var headers = new Dictionary<string, object>
			{
				// each event in this operation will be associated with the same commit
				{"CommitId", commitId},
				// store type of class so event can be rebuilt when the event is loaded
				{
					EventClrTypeHeader,
					domainEvent.GetType().AssemblyQualifiedName
				}
			};
			// events and headers are stored at binary-encoded JSON
			var data =
				Encoding.UTF8.GetBytes(
					JsonConvert.SerializeObject(domainEvent));
			var metadata = Encoding.UTF8.GetBytes(
				JsonConvert.SerializeObject(headers)
			);

			// enhanced support in the admin web UI if Event Store knows events are JSON
			var isJson = true;

			return new EventData(
				eventId,
				domainEvent.GetType().Name,
				isJson,
				data,
				metadata);
		}


		public async Task<List<IDomainEvent>> GetStreamAsync(
			string streamName,
			int fromVersion,
			int toVersion)
		{
			var amount = (toVersion - fromVersion) + 1;
			var events = await _esConn.ReadStreamEventsForwardAsync(StreamName
				(streamName), fromVersion, amount, false);


			return events.Events
				.Select(e => (IDomainEvent)RebuildEvent(e))
				.ToList();
		}

		public async Task<T> GetLatestSnapshotAsync<T>(string streamName) where T : 
		class
		{
			var stream = SnapshotStreamNameFor(streamName);
			var amountToFetch = 1;
			var ev = await _esConn.ReadStreamEventsBackwardAsync(stream,
				StreamPosition.End, amountToFetch, false);

			if (ev.Events.Any())
			{
				return (T) RebuildEvent(ev.Events.Single());
			}

			return null;
		}

		public async Task AddSnapshotAsync<T>(string streamName,
			T snapshot)
		{
			var stream = SnapshotStreamNameFor(streamName);

			var snapshotAsEvent = MapToEventStoreStorageFormat(
				snapshot,
				Guid.NewGuid(),
				Guid.NewGuid()
			);

			await _esConn.AppendToStreamAsync(stream, ExpectedVersion.Any,
				snapshotAsEvent);
		}

		private string SnapshotStreamNameFor(string streamName)
		{
			return StreamName(streamName) + "-snapshots";
		}

		private object RebuildEvent(ResolvedEvent @event)
		{
			var metadata = @event.OriginalEvent.Metadata;
			var data = @event.OriginalEvent.Data;
			var typeOfDomainEvent = JObject
				.Parse(Encoding.UTF8.GetString(metadata))
				.Property(EventClrTypeHeader).Value;
			var rebuildEvent = JsonConvert.DeserializeObject(
				Encoding.UTF8.GetString(data),
				Type.GetType((string) typeOfDomainEvent)
			);
			return rebuildEvent;
		}
	}
}