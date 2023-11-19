using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class MapDrawer
    {

        public MapDrawer() {
            InitialiseScreen();
            DrawMap();
            MapLogic();
        }
        private void InitialiseScreen() {
            Program.game.screen.ClearForMap();
        }

        private void MapLogic() {
            while(true) {

            }
        }

        private void DrawMap() {
            
        }
    }
}