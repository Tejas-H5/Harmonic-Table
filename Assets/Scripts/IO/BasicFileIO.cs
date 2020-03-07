using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class BasicFileIO {
    public static string exception = "No errors";

	//creates the following path if it doesn't exist.
    public static bool EnsureDirectory(string path){
    	try{
    		if(!Directory.Exists(path)){
                Directory.CreateDirectory(path);
            }
    	} catch (Exception e){
            exception = e.ToString() + "\n\n" + e.Message + "\n\n" + e.StackTrace;
    		return false;
    	}

    	return true;
    }

    public static IEnumerable<string> EnumerateFiles(string path, string fileExtension){
    	if(!EnsureDirectory(path)){
    		return null;
    	}
    	return Directory.EnumerateFiles(path, "*." + fileExtension);
    }
}
