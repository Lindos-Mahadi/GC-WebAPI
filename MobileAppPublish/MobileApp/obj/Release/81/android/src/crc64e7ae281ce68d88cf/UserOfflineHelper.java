package crc64e7ae281ce68d88cf;


public class UserOfflineHelper
	extends android.database.sqlite.SQLiteOpenHelper
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/database/sqlite/SQLiteDatabase;)V:GetOnCreate_Landroid_database_sqlite_SQLiteDatabase_Handler\n" +
			"n_onUpgrade:(Landroid/database/sqlite/SQLiteDatabase;II)V:GetOnUpgrade_Landroid_database_sqlite_SQLiteDatabase_IIHandler\n" +
			"";
		mono.android.Runtime.register ("PMS.Droid.Classes.OffLineHelpers.UserOfflineHelper, MobileApp", UserOfflineHelper.class, __md_methods);
	}


	public UserOfflineHelper (android.content.Context p0, java.lang.String p1, android.database.sqlite.SQLiteDatabase.CursorFactory p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == UserOfflineHelper.class)
			mono.android.TypeManager.Activate ("PMS.Droid.Classes.OffLineHelpers.UserOfflineHelper, MobileApp", "Android.Content.Context, Mono.Android:System.String, mscorlib:Android.Database.Sqlite.SQLiteDatabase+ICursorFactory, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onCreate (android.database.sqlite.SQLiteDatabase p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.database.sqlite.SQLiteDatabase p0);


	public void onUpgrade (android.database.sqlite.SQLiteDatabase p0, int p1, int p2)
	{
		n_onUpgrade (p0, p1, p2);
	}

	private native void n_onUpgrade (android.database.sqlite.SQLiteDatabase p0, int p1, int p2);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
