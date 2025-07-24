// ! GET FROM: https://forum.unity.com/threads/helpattribute-allows-you-to-use-helpbox-in-the-unity-inspector-window.462768/

using UnityEngine;
     
public enum HelpBoxMessageType { None, Info, Warning, Error }
     
public class HelpBoxAttribute : PropertyAttribute 
{
    public string text;
    public HelpBoxMessageType messageType;
     
    public HelpBoxAttribute(string text, HelpBoxMessageType messageType = HelpBoxMessageType.None) 
    {
        this.text = text;
        this.messageType = messageType;
    }
}