/**
* <description>
*     Karma - everyonce port of hubot-plusplus and scorekeeper
* </description>
*
* <configuration>
*
* </configuration>
*
* <commands>
*     mmbot karma - shows the karma stats of users
* </commands>
* 
* <author>
*     everyonce
* </author>
*/

using System.Text.RegularExpressions;

var robot = Require<Robot>();
var karmaBrain = robot.Brain.Get<Dictionary<String,Int32>>("karma").Result ?? new Dictionary<String,Int32>();

robot.Hear(@"^([\s\w'@.-:]*)\s*([-+]{2}|—)(?:\s+(?:for|because|cause|cuz)?\s+(.+))?$", (msg) => 
{
    var Dummy = msg.Match[0];
    var Name = msg.Match[1];
    var Operator = msg.Match[2];
    var Reason = msg.Match[3].Trim().ToLower();
    var From = msg.Message.User.Name.ToLower();
    var Room = msg.Message.User.Room.Trim().ToLower();
    Name = Regex.Replace(Name, @"(^\s*@)|([,:\s]*$)", "");
    
    if (!karmaBrain.ContainsKey(Name)) karmaBrain.Add(Name,1);
    if (Operator == "++")
        karmaBrain[Name]++;
    else
        karmaBrain[Name]--;
    msg.Send(string.Format("{0} now has {1} karma.", Name, karmaBrain[Name]));
    robot.Brain.Set("karma", karmaBrain);
});
robot.Respond(@"karma\s*(best|worst)?", (msg) =>
{
    var order = msg.Match[1];
    foreach (var thing in karmaBrain)
       msg.Send(string.Format("{0} now has {1} karma.", thing.Key, thing.Value));
});

