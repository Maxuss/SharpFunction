namespace SharpFunction.Universal
{
    /// <summary>
    ///     Represents passenger of entity
    /// </summary>
    public sealed class EntityPassenger
    {
        /// <summary>
        ///     Initialize new passenger
        /// </summary>
        /// <param name="id">Spawn ID of mob</param>
        /// <param name="nbt">Non-wrapped NBT data of mob. Can be made with EntityNBT.Compile()</param>
        public EntityPassenger(string id, string nbt = "")
        {
            ID = id;
            NBT = nbt;
        }

        /// <summary>
        ///     ID of mob
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        ///     NBT data of mob
        /// </summary>
        public string NBT { get; set; } = "";
    }
}