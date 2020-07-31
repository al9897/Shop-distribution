using UnityEngine;
using UnityEditor;

public static class exportButtonHandler {
    [MenuItem("export Button Handler/Add To Report %F1")]
	static void DEV_AppendToReport(){
		//CSVManager.reportFileName += "report" + CSVManager.GetTimeStamp() + ".csv";
		CSVManager.AppendToReport(
			new string[4]
			{
				"An",
				Random.Range(0,1999).ToString(),
				Random.Range(0,101).ToString(),
				Random.Range(0,101).ToString()
			}
			);
		EditorApplication.Beep();
	}
	
	
}
