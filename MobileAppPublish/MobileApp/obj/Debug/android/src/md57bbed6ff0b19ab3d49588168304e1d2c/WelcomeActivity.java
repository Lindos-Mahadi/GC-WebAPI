package md57bbed6ff0b19ab3d49588168304e1d2c;


public class WelcomeActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("PMS.Droid.WelcomeActivity, MobileApp", WelcomeActivity.class, __md_methods);
	}


	public WelcomeActivity ()
	{
		super ();
		if (getClass () == WelcomeActivity.class)
			mono.android.TypeManager.Activate ("PMS.Droid.WelcomeActivity, MobileApp", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
