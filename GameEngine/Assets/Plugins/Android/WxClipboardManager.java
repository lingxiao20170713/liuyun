package com.HongQu.ZhangMen;

import com.unity3d.player.*;

import android.app.Activity;
import android.content.ClipData;
import android.content.ClipDescription;
import android.content.ClipboardManager;
import android.content.Context;

public class WxClipboardManager 
{
	public static ClipboardManager mClipboardManager = null;
	Activity mActivity;
	
//	public void OnCreate(Context ctx)
//	{
//		
//		 mActivity = (Activity)ctx;
//		 
//		mActivity.runOnUiThread(new Runnable()
//		{ 
//			@Override
//			public void run() 
//			{
//		    	if (mClipboardManager == null)
//		    	{
//		    		mClipboardManager = (ClipboardManager) mActivity.getSystemService(Activity.CLIPBOARD_SERVICE);
//		    	}
//			}
//		});
//	}
	
    public static void CopyTextToClipboard(final Context activity, final String text) throws Exception 
    {
    	final Activity mActivity = (Activity)activity;
    	if (mClipboardManager == null)
    	{
    		mActivity.runOnUiThread(new Runnable()
    		{ 
    			@Override
    			public void run() 
    			{

    				mClipboardManager = (ClipboardManager) activity.getSystemService(Activity.CLIPBOARD_SERVICE);
    			}
    		});
    		

    	}


        ClipData clipData = ClipData.newPlainText("data", text);
        mClipboardManager.setPrimaryClip(clipData);    

			
		
    }    

    public static String GetTextFromClipboard(final Context activity) 
    {
    	
    	final Activity mActivity = (Activity)activity;
    	if (mClipboardManager == null)
    	{
    		mActivity.runOnUiThread(new Runnable()
    		{ 
    			@Override
    			public void run() 
    			{
    				mClipboardManager = (ClipboardManager) activity.getSystemService(Activity.CLIPBOARD_SERVICE);
    			}
    		});		

    	} 

        if (mClipboardManager != null && mClipboardManager.hasPrimaryClip()&& mClipboardManager.getPrimaryClipDescription().hasMimeType(ClipDescription.MIMETYPE_TEXT_PLAIN))
        {
            ClipData clipData = mClipboardManager.getPrimaryClip();
            ClipData.Item item = clipData.getItemAt(0);
            return item.getText().toString();
        }
		
        return null;

    }
}
