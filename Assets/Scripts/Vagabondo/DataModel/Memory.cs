using System;

namespace Vagabondo.DataModel
{
    public enum MemoryType
    {
        QuestFragment,
        Quest,
    }

    public class Memory
    {
        public MemoryType type;
        public string title;
        public string description;

        public Memory(MemoryType type)
        {
            this.type = type;
        }
    }

    public class QuestMemory : Memory
    {
        public Guid questId;

        public QuestMemory() : base(MemoryType.Quest) { }
    }

    public class QuestFragmentMemory : Memory
    {
        public Guid questId;

        public QuestFragmentMemory() : base(MemoryType.QuestFragment) { }
    }
}