using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Spextria.Entities.EntityParts;

namespace Spextria.Statics
{
    static class Npcs
    {
        public static Dictionary<string, List<Dialog>> NpcDialogs = new Dictionary<string, List<Dialog>>();
        
        public static void LoadNpcInfo()
        {
            NpcDialogs.Add("man_by_campfire", new List<Dialog>()
            {
                new Dialog(3, "HELLO THERE, STRANGER...  #grey#CLICK ANYWHERE TO READ THE NEXT DIALOG"),
                new Dialog(4, "THIS #yellow#GOLDEN WOOD #white#MAKES GREAT FIREWOOD, IT CATCHES #orange#FIRE #white#VERY EASILY!"),
                new Dialog(3, "HERE, TAKE THIS. IT MIGHT HELP YOU ON YOUR JOURNEY.", "!has_weapon", "torch", true, "give_weapon", "torch"),
                new Dialog(4, "YOU CAN OPEN THE WEAPON MENU BY CLICKING THAT BUTTON WITH A FIST ON THE LEFT SIDE OF THE SCREEN"),
                new Dialog(1, "I'LL BE HERE... ALONE...")
            });
        }
    }
}
