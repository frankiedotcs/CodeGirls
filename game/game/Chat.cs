using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SignalRChat {
    public class Chat {
        private Timer timer;
        public int[,] gridArray = new int[12, 12];
        public int[] array1D = new int[144];
        SQLCode sql = new SQLCode();
        DBConnect db = new DBConnect();
        public int count = 0;

        //SINGLETON-RELATED STUFF
        // Singleton instance - can only be created once because it's private and static 
        private readonly static Lazy<Chat> instance = new Lazy<Chat>(() =>
            new Chat(GlobalHost.ConnectionManager.GetHubContext<ChatHub>().Clients));

        //SINGLETON-RELATED STUFF
        private Chat(IHubConnectionContext<dynamic> clients) {
            Clients = clients;
        }

        //SINGLETON-RELATED STUFF
        /// Singleton instance of the SignalRTime class.
        public static Chat Instance {
            get {
                return instance.Value;
            }
        }

        //SINGLETON-RELATED STUFF
        /// Clients from the Hub which will be used to broadcast time updates.
        private IHubConnectionContext<dynamic> Clients {
            get;
            set;
        }

        //Updates the gridarray by one step
        public void Update() {
            int[,] newArray = gridArray.Clone() as int[,];

            for (int gridRow = 1; gridRow < 11; gridRow++) {
                for (int gridCol = 1; gridCol < 11; gridCol++) {
                    int count = CheckNeighbors(gridRow, gridCol);

                    if (count == 3)
                        newArray[gridRow, gridCol] = 1;

                    if (count > 3 || count < 2)
                        newArray[gridRow, gridCol] = 0;
                }
            }

            gridArray = newArray.Clone() as int[,];
            Array2dtoArray1d(gridArray);
        }

        //Called on timer interval
        //Makes call to update gridarray then updates array on all clients
        public void UpdateClients(Object state) {
            Update();
            Clients.All.updateArrayOnPage(array1D);
        }
        //Keeps count on number of neighbors cell has
        public int CheckNeighbors(int r, int c) {
            int count = 0;
            //check if on edge
            if (r != 0 && r != 12 && c != 0 && c != 12) {
                //starting in upper left corner, checking clock-wise
                if (IsAlive(r - 1, c - 1)) count++;
                if (IsAlive(r - 1, c)) count++;
                if (IsAlive(r - 1, c + 1)) count++;
                if (IsAlive(r, c + 1)) count++;
                if (IsAlive(r + 1, c + 1)) count++;
                if (IsAlive(r + 1, c)) count++;
                if (IsAlive(r + 1, c - 1)) count++;
                if (IsAlive(r, c - 1)) count++;
            }
            return count;
        }

        //Checks if cell is alive
        public bool IsAlive(int r, int c) {
            if (gridArray[r, c] == 1)
                return true;
            else
                return false;
        }

        //Initializes neighborhood to glider formation
        public void GliderArray() {
            for (int r = 0; r < 12; r++) {
                for (int c = 0; c < 12; c++) {
                    if ((r == 6 && (c == 4 || c == 5 || c == 6)) ||
                        (r == 5 && c == 6) ||
                        (r == 4 && c == 5)) {
                        gridArray[r, c] = 1;
                    }
                    else {
                        gridArray[r, c] = 0;
                    }
                }
            }
            Array2dtoArray1d(gridArray);
        }

        //Update a signle cell on user click
        public void UpdateCell(int r, int c) {
            gridArray[r, c] = 1;
            Array2dtoArray1d(gridArray);
        }


        //Start global timer to update cells every 1 sec
        public void StartTimer() {
            timer = new Timer(UpdateClients, null, 0, 1000);
        }

        //Stops global timer
        public void StopTimer() {
            if (timer != null)
                timer.Dispose();
        }

        //Increments count of clients connected
        public void IncCount() {
            count++;
        }

        //Decrements count of clients connected
        public void DecCount() {
            count--;
        }

        ////Getters
        public int[,] GetGridArray() {
            return gridArray;
        }
        public int GetCount() {
            return count;
        }
        public int[] GetArray1d() {
            return array1D;
        }

        //Converts inputed 2d array to gridArray 1d array
        public void Array2dtoArray1d(int[,] arr) {
            int count = 0;
            for (int r = 0; r < 12; r++) {
                for (int c = 0; c < 12; c++) {
                    array1D[count] = gridArray[r, c];
                    count++;
                }
            }
        }


    }

}