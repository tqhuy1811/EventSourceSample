fromStream('some-stream')
	.when({
		"some-event": function (s, e) {
			var currentCpu = e.body["sys-cpu"];
			if (currentCpu > 20) {
				// emit event to a different stream with custom event
				// heavycpu is new stream, heavyCpuFound is custom event
				// Use case: When this event happens and this information is on the event, trigger a new event to a different stream.
				emit("heavycpu", "heavyCpuFound", {
					"level": currentCpu
				})
			}
		}
	});

fromStream('$stats-127.0.0.1:2113')
	.when({
		"some-event": function (s, e) {
			var currentCpu = e.body["sys-cpu"];
			if (currentCpu > 40) {
				// create initial state
				if (!s.count) s.count = 0;
				s.count += 1;
				// emit to different stream
				if (s.count >= 3)
					emit("heavycpu", "heavyCpuFound", {
						"level": currentCpu,
						"count": s.count
					});
			} else {
				s.count = 0;
			}
		}
	});

fromAll().when({
	$any: function (s, e) {
		// indexing events => create pointer to event and write those pointers to a different stream
		// emit will just write events directly 
		linkTo(e.metadata.username, e);
	},
});

// use this for distributed streams, so streams can be process in order
options({
	reorderEvents: true,
	processingLag: 500 //time in ms
});


fromCategory('strategy')
	.foreachStream() // run when filter on  all stream in parallel 
	.when({
		// because of foreachStream => each stream has its own states
		$init: function () {
			return {
				"id": 0,
				"bidsPlaced": 0,
				"bidsWon": 0,
				"goodthings": 0
			}
		},
		StrategyStarted: function (s, e) {
			s.id = e.body.strategyid
		},
		BidPlaced: function (s, e) {
			s.bidsPlace += 1;
		},
		BidWon: function (s, e) {
			s.bidsWon += 1;
		},
		SomethingGoodHappenned: function (s, e) {
			s.goodthings++;
			linkTo('goodthings-' + s.id, e);
		},

		IntervalOccurred: function (s, e) {
			emit('liveresults-' + s.id,
				{
					"strategyid": s.id,
					"goodthings": s.goodthings,
					"bidsPlaced": s.bidsPlaced,
					"bidsWon": s.bidsWon,
					"time": e.time
				});
			s.bidsPlaced = 0;
			s.bidsWon = 0;
			s.goodthings = 0;
		}
	});