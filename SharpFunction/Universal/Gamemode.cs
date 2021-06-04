using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpFunction.Universal.EnumHelper;
namespace SharpFunction.Universal
{
    /// <summary>
    /// Represents Minecraft's game mode
    /// </summary>
    public enum Gamemode
    {
        [EnumValue("survival")] Survival,
        [EnumValue("creative")] Creative,
        [EnumValue("adventure")] Adventure,
        [EnumValue("spectator")] Spectator
    }
}
