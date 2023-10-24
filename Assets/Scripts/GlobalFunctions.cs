//**************************************************************************************
// File: GlobalFunctions.cs
//
// Purpose: This is used to contain generic static functions
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

public class GlobalFunctions
{
    //***********************************************************************
    // Method: IsNull
    //
    // Purpose: This is a generalized null check function that should work
    // in all cases.. since unity is strange with how it's objects pretend
    // to be null and trying to directly check for null with a
    // System.Object will throw an exception
    //***********************************************************************
    public static bool IsNull(System.Object pObj)
    {
        return pObj == null || pObj.Equals(null);
    }
}