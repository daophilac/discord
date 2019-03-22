package com.daophilac.discord;


import android.Manifest;
import android.app.Activity;
import android.content.pm.PackageManager;
import android.support.v4.app.ActivityCompat;

public final class PermissionInquirer {
    private static final int REQUEST_EXTERNAL_STORAGE = 1;
    private static final int REQUEST_CAMERA = 1;
    private static final int REQUEST_LOCATION = 1;
    private static final String[] PERMISSIONS_STORAGE = {Manifest.permission.READ_EXTERNAL_STORAGE, Manifest.permission.WRITE_EXTERNAL_STORAGE};
    private static final String[] PERMISSIONS_CAMERA = {Manifest.permission.CAMERA};
    private static final String[] PERMISSIONS_LOCATION = {Manifest.permission.LOCATION_HARDWARE};
    public static void askStoragePermission(Activity activity){
        int permission = ActivityCompat.checkSelfPermission(activity, Manifest.permission.WRITE_EXTERNAL_STORAGE);
        if(permission != PackageManager.PERMISSION_GRANTED){
            ActivityCompat.requestPermissions(activity, PERMISSIONS_STORAGE, REQUEST_EXTERNAL_STORAGE);
        }
    }
    public static void askCameraPermission(Activity activity){
        int permission = ActivityCompat.checkSelfPermission(activity, Manifest.permission.CAMERA);
        if(permission != PackageManager.PERMISSION_GRANTED){
            ActivityCompat.requestPermissions(activity, PERMISSIONS_CAMERA, REQUEST_CAMERA);
        }
    }

}
