using System.Collections.Generic;
using System.Linq;
using console = System.Console;

namespace MyFeedlyClient.Console
{
    class Program
    {
        private static Program _program;
        private static readonly Client _client = new Client();
        private static HashSet<ActionTuple> _actions;

        private Program()
        {
            _program = this;
        }

        static void Main(string[] args)
        {
            _actions = GetActions();

            while (true)
            {
                DisplayActions();

                if (int.TryParse(console.ReadLine(), out var key))
                {
                    var action = _actions.FirstOrDefault(a => a.Key == key);
                    action?.Action();
                }
            }
        }

        private static void DisplayActions()
        {
            _program.Border();

            foreach (var action in _actions)
                _program.WriteAction(action.Action.Method.Name, action.Key);
        }

        private static HashSet<ActionTuple> GetActions()
        {
            return new HashSet<ActionTuple>
            {
                new ActionTuple
                {
                    Action = _client.Login,
                    Key = 1
                },
                new ActionTuple
                {
                    Action = _client.GetAllUsers,
                    Key = 2
                },
                new ActionTuple
                {
                    Action = _client.GetUser,
                    Key = 3
                },
                new ActionTuple
                {
                    Action = _client.GetUserWithCollections,
                    Key = 4
                },
                new ActionTuple
                {
                    Action = _client.CreateUser,
                    Key = 5
                },
                new ActionTuple
                {
                    Action = _client.UpdateUser,
                    Key = 6
                },
                new ActionTuple
                {
                    Action = _client.DeleteUser,
                    Key = 7
                },
                new ActionTuple
                {
                    Action = _client.CreateCollection,
                    Key = 8
                },
                new ActionTuple
                {
                    Action = _client.GetCollectionById,
                    Key = 9
                },
                new ActionTuple
                {
                    Action = _client.GetNewsByCollectionId,
                    Key = 10
                },
                new ActionTuple
                {
                    Action = _client.GetAllFeeds,
                    Key = 11
                },
                new ActionTuple
                {
                    Action = _client.GetFeedById,
                    Key = 12
                },
                new ActionTuple
                {
                    Action = _client.CreateFeed,
                    Key = 13
                },
                new ActionTuple
                {
                    Action = _client.Exit,
                    Key = 0
                }
            };
        }
    }
}
