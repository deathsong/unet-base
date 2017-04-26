using UnityEngine;
using System.Collections;


public class RichString  {

	public bool italic;
	public bool bold;

	public bool colored;
	private string color="";

	public string text="";

	public RichString()
	{
		
	}

	public RichString(string _text)
	{
		text  = _text;
	}

	public string Color{
		get {
			return color;
		}
		set {
			color = value;
			colored = true;
		}
	}
		

	public string FromStyle(RichString rs)
	{
		string output = "";
		if (rs.bold) {
			output += "<b>";
		}
		if (rs.italic) {
			output += "<i>";
		}
		if (rs.colored) {
			output += "<color=" + rs.Color + ">";
		}

		output += rs.text;

		if (rs.colored) {
			output += "</color>";
		}
		if (rs.italic) {
			output += "</i>";
		}
		if (rs.bold) {
			output += "</b>";
		}
		return output;	
	}

	public static implicit operator RichString(string input)
	{
		RichString rs = new RichString (input);
		return rs;
	}

	public static implicit operator string(RichString rs)
	{
		string output = "";
		if (rs.bold) {
			output += "<b>";
		}
		if (rs.italic) {
			output += "<i>";
		}
		if (rs.colored) {
			output += "<color=" + rs.color + ">";
		}

		output += rs.text;

		if (rs.colored) {
			output += "</color>";
		}
		if (rs.italic) {
			output += "</i>";
		}
		if (rs.bold) {
			output += "</b>";
		}
		return output;
	}

}
