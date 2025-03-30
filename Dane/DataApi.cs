namespace Dane
{
    public abstract class DataApi
    { 
       public static DataApi CreateNewInstance()
       {
           return new DataAPI();
       }

       internal sealed class DataAPI : DataApi
       {

       }
    }
}