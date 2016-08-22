using Pathfinding;

namespace Rpg.PathFinding
{
    class StaticTagsPathConstraint : PathNNConstraint
    {
        // Do nothing when setting tags.
        public new int tags { get { return base.tags; } set { base.tags = 99; } }

        public StaticTagsPathConstraint(int tags)
        {
            base.tags = tags;
        }
    }
}
