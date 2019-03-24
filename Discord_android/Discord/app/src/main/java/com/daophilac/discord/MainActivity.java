package com.daophilac.discord;

import android.content.Context;
import android.content.ContextWrapper;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Environment;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.annotation.NonNull;
import android.support.design.widget.NavigationView;
import android.support.v4.app.FragmentManager;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.Toast;

import com.daophilac.discord.customview.ServerButton;

import java.io.File;
import java.util.Locale;

public class MainActivity extends AppCompatActivity {
    public static final String LOG_TAG = "com.daophilac.discord";
    public static Locale locale;
    private Inventory inventory;
    private DrawerLayout drawerLayout;
    private NavigationView navigationView;
    private Toolbar toolbar;
    private ActionBar actionBar;
    private FragmentManager fragmentManager;
    private NavigatorFragment navigatorFragment;
    public Handler backgroundHandler;
    private Thread threadBackground;
    private APICaller apiCaller;
    private String baseURL;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        initializeGlobalVariable();
        this.inventory.storeUser(this.getIntent().getStringExtra("jsonUser"));
        writeLogConsole(this.inventory.loadUser().getEmail());
        this.fragmentManager.beginTransaction().replace(R.id.navigation_view, navigatorFragment).commit();////////////////////
//        ServerButton buttonabc = findViewById(R.id.buttonabc);
//        buttonabc.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                MainActivity.writeLogConsole("aaaaaaaa");
//            }
//        });
//        Button buttonSignOut = findViewById(R.id.buttonSignOut);
//        buttonSignOut.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                //deleteAppData();
//                abc();
//            }
//        });
        //deleteAppData();
    }
    private void initializeGlobalVariable(){
        locale = Locale.ENGLISH;// TODO:
        this.inventory = new Inventory();
        this.drawerLayout = findViewById(R.id.drawer_layout);
        this.navigationView = findViewById(R.id.navigation_view);
        //this.drawerLayout.setDrawerLockMode(DrawerLayout.LOCK_MODE_LOCKED_OPEN, this.navigationView);







//        NavigationView navigationView = (NavigationView) findViewById(R.id.n);
//        navigationView.setNavigationItemSelectedListener(new NavigationView.OnNavigationItemSelectedListener() {
//            @Override
//            public boolean onNavigationItemSelected(@NonNull MenuItem item) {
//                return true;
//            }
//        });







        this.toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        this.actionBar = getSupportActionBar();
        this.actionBar.setDisplayHomeAsUpEnabled(true);
        this.actionBar.setHomeAsUpIndicator(R.drawable.ic_navigator);

        this.navigatorFragment = new NavigatorFragment();
        this.navigatorFragment.setInventory(this.inventory);
        this.fragmentManager = getSupportFragmentManager();
        this.baseURL = "http://" + Route.serverIP + "/" + Route.serverName;
        this.apiCaller = new APICaller();
    }


    private void abc(){
        Intent intent = new Intent(this, ThirdActivity.class);
        startActivity(intent);
    }
    private void deleteDirectory(File fileOrDirectory){
        if (fileOrDirectory.isDirectory()){
            for (File child : fileOrDirectory.listFiles()){
                deleteDirectory(child);
            }
        }
        fileOrDirectory.delete();
    }
    private void deleteAppData() {
        ContextWrapper contextWrapper = new ContextWrapper(this);
        File directory = new File(contextWrapper.getFilesDir() + "/" + getString(R.string.account_internal_directory));
        deleteDirectory(directory);
        Toast.makeText(this,"Deleted all data", Toast.LENGTH_LONG).show();
    }
    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                drawerLayout.openDrawer(GravityCompat.START);
        }
        return super.onOptionsItemSelected(item);
    }
    public static void writeLogConsole(String message){
        Log.v(LOG_TAG, message);
    }
}
