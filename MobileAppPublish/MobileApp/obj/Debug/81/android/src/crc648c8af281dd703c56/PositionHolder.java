package crc648c8af281dd703c56;


public class PositionHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("PMS.Droid.PositionHolder, MobileApp", PositionHolder.class, __md_methods);
	}


	public PositionHolder ()
	{
		super ();
		if (getClass () == PositionHolder.class)
			mono.android.TypeManager.Activate ("PMS.Droid.PositionHolder, MobileApp", "", this, new java.lang.Object[] {  });
	}

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
