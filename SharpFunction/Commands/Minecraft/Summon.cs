using SharpFunction.Universal;

namespace SharpFunction.Commands.Minecraft
{
    /// <summary>
    ///     Represents Summon command. Equal to Minecraft's <code>/summon {entity: entity_id} {pos: Vector3} {nbt: NBT}</code>
    /// </summary>
    public sealed class Summon : ICommand
    {
        public string Compiled { get; private set; }

        /// <summary>
        ///     Compiles the /summon command
        /// </summary>
        /// <param name="entity">Entity to summon</param>
        /// <param name="position">Position of entity to summon</param>
        public void Compile(Entity entity, Vector3 position)
        {
            var nbt = entity.NBT;
            var name = entity.ID;
            var pos = position.String();

            Compiled = $"/summon {name} {pos} {nbt}";
        }
    }
}