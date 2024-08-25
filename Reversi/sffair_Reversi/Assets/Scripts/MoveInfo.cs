using System.Collections.Generic;

/**
 * Game built with help from OttoBotCode of YouTube
 */
public class MoveInfo
{
   public Player Player {  get; set; }
   public Position Position { get; set; }
   public List<Position> OutFlanked {  get; set; }
}
