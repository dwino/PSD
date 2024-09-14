namespace Balance;

public class InteractionNode
{
    public InteractionNode(int nodeID, string text, List<InteractionOption> interactionOptions,
                            List<string> removeFlags, List<string> addFlags)
    {
        NodeID = nodeID;
        Text = text;
        InteractionOptions = interactionOptions;
        CurrentOptionIndex = -1;
        RemoveFlags = removeFlags;
        AddFlags = addFlags;
    }

    public int NodeID = -1; //I use -1 as a way to exit a conversation.
                            //NodeId should be a positive number

    public string Text;
    public List<string> RemoveFlags { get; set; }
    public List<string> AddFlags { get; set; }

    public List<InteractionOption> InteractionOptions;
    public int CurrentOptionIndex { get; set; }
    public InteractionOption CurrentOption
    {
        get
        {
            if (CurrentOptionIndex != -1 && CurrentOptionIndex < InteractionOptions.Count)
            {
                return InteractionOptions[CurrentOptionIndex];
            }
            else
            {
                return null;
            }
        }
    }


}
