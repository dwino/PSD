namespace Balance;

public class InteractionOption
{
    public InteractionOption(string text, int destinationNodeID)
    {
        Text = text;
        DestinationNodeID = destinationNodeID;
    }

    public string Text;
    public int DestinationNodeID;
}
