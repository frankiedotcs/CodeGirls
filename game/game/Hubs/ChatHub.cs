using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Timers;

namespace SignalRChat {
    public class ChatHub : Hub {
        public int[,] gridArray = new int[12, 12];
        public int[] array1D = new int[144];
        public string arrayString;
        private readonly Chat chat;
        int count;

        //SINGLETON-RELATED STUFF
        //Creates signleton instance
        public ChatHub() : this(Chat.Instance) { }
        public ChatHub(Chat ch) {
            chat = ch;
        }

        public override Task OnConnected() {
            string name = Context.User.Identity.Name;

            //Increments count everytime client connects
            chat.IncCount();
            count = chat.GetCount();

            //Initiliaze grid to glider formation if first client to be connected
            if (count == 1) {
                chat.StopTimer();
                chat.GliderArray();
            }

            //Update array on clients
            array1D = chat.GetArray1d();
            Clients.All.updateArrayOnPage(array1D);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled) {
            //Decrements count everytime client disconnects
            chat.DecCount();
            return base.OnDisconnected(stopCalled);
        }



        //Client calls to execute next step in game logic 
        public void Next() {
            string name = Context.User.Identity.Name;
            chat.Update();

            //Update array on clients
            array1D = chat.GetArray1d();
            Clients.All.updateArrayOnPage(array1D);
        }

        //Client calls when Hit Test is triggered 
        public void Change(int row, int col) {
            string name = Context.User.Identity.Name;
            chat.UpdateCell(row, col);

            //Update array on clients
            array1D = chat.GetArray1d();
            Clients.All.updateArrayOnPage(array1D);
        }

        //--Start, stop, and restarts buttons--//
        public void Start() {
            chat.StartTimer();
        }

        public void Stop() {
            chat.StopTimer();
        }

        public void Restart() {
            chat.StopTimer();
            chat.GliderArray();

            //Update array on clients
            array1D = chat.GetArray1d();
            Clients.All.updateArrayOnPage(array1D);
        }



    }//end class
}