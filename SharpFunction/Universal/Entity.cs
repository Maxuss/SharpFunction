namespace SharpFunction.Universal
{
    /// <summary>
    ///     Represents data stored within a single entity
    /// </summary>
    public sealed class Entity
    {
        /// <summary>
        ///     Initializes an entity without the nbt data
        /// </summary>
        /// <param name="id">Namespaced id of entity</param>
        public Entity(string id)
        {
            ID = id;
        }

        /// <summary>
        ///     Initializes an entity without the nbt data
        /// </summary>
        /// <param name="id">Namespaced id of entity</param>
        /// <param name="nbt">Wrapped and JSONified NBT data of entity</param>
        public Entity(string id, string nbt)
        {
            ID = id;
            NBT = nbt;
        }

        /// <summary>
        ///     Represents namespaced id of entity
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        ///     NBT data of entity
        /// </summary>
        public string NBT { get; set; } = "";
    }
}