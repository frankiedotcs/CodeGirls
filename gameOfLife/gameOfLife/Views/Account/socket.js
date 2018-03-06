// Apply FF/Mozilla patch.
        window.WebSocket = window.WebSocket || window.MozWebSocket;

        var url = "ws://localhost:8888/";
        var ws = new WebSocket(url);

        ws.onopen = function() {
            log("Socket opened.");
            ws.send("Opening websocket...");
        }
        
        ws.onmessage = function(e) {
            log("Incoming message:" + e.data);
        }
        
        ws.onclose = function(e) {
            log("Socket closed.");
        }

        function doSend(arr) {
            //var clientTextElem = document.getElementById("clientText");
            //var text = clientTextElem.value;
            
            log("Received array of length " + arr.length);
            ws.send(text);
        }
            
        function log(message) {
            var resultsElem = document.getElementById("results");
            resultsElem.innerHTML += message + "<br/>";
        }
