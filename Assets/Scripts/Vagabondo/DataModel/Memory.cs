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

        public QuestFragmentMemory(Guid questId) : base(MemoryType.QuestFragment)
        {
            this.questId = questId;
        }
    }
}
