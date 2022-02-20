using System.Collections.Immutable;

namespace GeneticBoilerplate
{
    public class Client
    {
        public ImmutableHashSet<string> Likes { get; }
        public ImmutableHashSet<string> Dislikes { get; }

        public Client(ImmutableHashSet<string> likes, ImmutableHashSet<string> dislikes)
        {
            Likes = likes;
            Dislikes = dislikes;
        }
    }
}