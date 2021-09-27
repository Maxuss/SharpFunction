namespace SharpFunction.Universal
{
    /// <summary>
    ///     Represents NBT Path as JSON string
    /// </summary>
    public sealed class NBTPath
    {
        /// <summary>
        ///     Initialize an NBT Path
        /// </summary>
        /// <param name="json">JSONed NBT data</param>
        public NBTPath(string json)
        {
            JSON = json;
        }

        /// <summary>
        ///     JSON string of NBT minecraft data
        /// </summary>
        public string JSON { get; set; }
    }
}