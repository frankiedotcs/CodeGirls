using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace SignalRChat{
    public class ChatHub : Hub{
        //TODO: Need to use global variable on database
        
            //This version recreates gridArray everytime client access hub
        public int[,] gridArray = new int[12, 12];
        public int[] array1D = new int[144];
        
        public void Send() {
             
            GliderArray();                  //TODO: Replace with call to database to get array
            Update();

            //convert 2d array to 1d array 
            int count = 0;
            for (int r = 0; r < 12; r++) {
                for (int c = 0; c < 12; c++) {
                    array1D[count] = gridArray[r, c];
                    count++;                       
                }
            }
            Clients.All.updateArrayOnPage(array1D);
        }

        //Client calls when Hit Test is triggered 
        public void CellClicked(int row, int col) {
            GliderArray();                  //TODO: Replace with call to database to get array

            //bring cell alive based on objects position
            gridArray[row, col] = 1;

            //convert 2d array to 1d array 
            int count = 0;
            for (int r = 0; r < 12; r++) {
                for (int c = 0; c < 12; c++) {
                    array1D[count] = gridArray[r, c];
                    count++;
                }
            }
            Clients.All.updateArrayOnPage(array1D);
        }


        public void GliderArray() {
            //Initializes neighborhood to glider formation
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
           // return gridArray;

        }

        //Checks neighborhood and brings alive/kills based on game logic
        public void Update() {
            int[,] newArray = gridArray.Clone() as int[,];

            for (int gridRow = 4; gridRow < 11; gridRow++) {
                for (int gridCol = 4; gridCol < 11; gridCol++) {
                    int count = CheckNeighbors(gridRow, gridCol);

                    if (count == 3)
                        newArray[gridRow, gridCol] = 1;

                    if (count > 3 || count < 2)
                        newArray[gridRow, gridCol] = 0;

                }
            }

            gridArray = newArray.Clone() as int[,];
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
            if (gridArray[r, c] == 1) {
                return true;
            }
            else {
                return false;
            }
        }

       



    }//end class
}