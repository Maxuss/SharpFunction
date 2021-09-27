namespace SharpFunction.Universal
{
    /// <summary>
    ///     Represents a Minecraft item
    /// </summary>
    public sealed class Item
    {
        /// <summary>
        ///     Initialize an item
        /// </summary>
        /// <param name="id">name-id of item</param>
        /// <param name="nbt">nbt data of an item</param>
        public Item(string id, ItemNBT nbt)
        {
            NBT = nbt;
            ID = id;
        }

        /// <summary>
        ///     Initialize an item without nbt data
        /// </summary>
        /// <param name="id">id-name of an item</param>
        public Item(string id)
        {
            ID = id;
        }

        /// <summary>
        ///     NBT data of item
        /// </summary>
        public ItemNBT NBT { get; set; }

        /// <summary>
        ///     ID of item
        /// </summary>
        public string ID { get; set; }

        public string GetCompiled()
        {
            var nb = !NullChecker.IsNull(NBT) ? $"{NBT.Compile()}" : "";
            return $"{ID}{nb}";
        }
    }
}