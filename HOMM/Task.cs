using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    interface Idwelling
    {
        int Owner { get; set; }
    }
    interface Iarmy
    {
        Army Army { get; set; }
    }
    interface ITreasure
    {
        Treasure Treasure { get; set; }
    }

    public class Dwelling : Idwelling
    {
        public int Owner { get; set; }
    }

    public class Mine : Idwelling, Iarmy, ITreasure
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : Iarmy, ITreasure
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : Iarmy
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasure
    {
        public Treasure Treasure { get; set; }
    }


    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            //if (mapObject is Dwelling dwellingObj)
            //{
            //    dwellingObj.Owner = player.Id;
            //    return;
            //}

            //if (mapObject is Mine mine)
            //{
            //    if (  xrmy))
            //    {
            //        mine.Owner = player.Id;
            //        player.Consume(mine.Treasure);
            //    }
            //    else player.Die();
            //    return;
            //}

            //if (mapObject is Creeps creeps)
            //{
            //    if (player.CanBeat(creeps.Army))
            //        player.Consume(creeps.Treasure);
            //    else
            //        player.Die();
            //    return;
            //}

            //if (mapObject is ResourcePile resourcePile)
            //{
            //    player.Consume(resourcePile.Treasure);
            //    return;
            //}

            //if (mapObject is Wolves wolves)
            //{
            //    if (!player.CanBeat(wolves.Army))
            //        player.Die();
            //}

            if (mapObject is Iarmy army)
            {
                if (!player.CanBeat(army.Army))
                {
                    player.Die();
                    return;
                }
            }

            if (mapObject is ITreasure treasure)
            {
                player.Consume(treasure.Treasure);
            }

            if (mapObject is Idwelling dwelling)
            {
                dwelling.Owner = player.Id;
            }
        }
    }
}
