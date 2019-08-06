package com.peanut.discord.tools;


import android.app.Activity;
import android.content.Context;
import android.content.pm.PackageManager;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import android.widget.Toast;

public final class PermissionInquirer {
    private Context context;
    public PermissionInquirer(Context context){
        this.context = context;
    }

    /**
     * @param permission must be the constant string from Manifest.permission.
     * @param permissionCode is the application constant defined integer number, used to determine the permission in the callback method onRequestPermissionsResult.
     * @param hasNotBeenGrantedMessage used to display a toast message if the permission has not been granted yet.
     */
    public void askPermission(String permission, int permissionCode, String hasNotBeenGrantedMessage){
        // Here, this.context is the current context
        if(ContextCompat.checkSelfPermission(this.context, permission) != PackageManager.PERMISSION_GRANTED){
            // ServerPermission is not granted
            // Should we show an explanation?
            if(ActivityCompat.shouldShowRequestPermissionRationale((Activity) this.context, permission)){
                // Show an explanation to the user *asynchronously* -- don't block // TODO:
                // this thread waiting for the user's response! After the user
                // sees the explanation, try again to request the permission.
                Toast.makeText(this.context, hasNotBeenGrantedMessage, Toast.LENGTH_LONG).show();
                ActivityCompat.requestPermissions((Activity) context, new String[]{permission}, permissionCode);
            }
            else{
                // No explanation needed; request the permission
                ActivityCompat.requestPermissions((Activity) context, new String[]{permission}, permissionCode);
            }
        }
        else {

        }
    }
    public void askPermission(String permission, int permissionCode){
        if(ContextCompat.checkSelfPermission(this.context, permission) != PackageManager.PERMISSION_GRANTED){
            ActivityCompat.requestPermissions((Activity) context, new String[]{permission}, permissionCode);
        }
        else {

        }
    }
    public boolean checkPermission(String permission){
        return ContextCompat.checkSelfPermission(this.context, permission) == PackageManager.PERMISSION_GRANTED;
    }
}