using System;

namespace SubSystems.SceneObjects
{
    public interface IBlockTrigger
    {
        event Action OnBlockPressed;
        event Action OnBlockReleased;
    }
}