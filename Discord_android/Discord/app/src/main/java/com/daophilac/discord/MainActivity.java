package com.daophilac.discord;

import android.content.ContextWrapper;
import android.os.Handler;
import android.os.Looper;
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
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.daophilac.discord.customview.MessageTextView;
import com.daophilac.discord.interfaces.MainActivityListener;
import com.daophilac.discord.interfaces.NavigatorListener;
import com.daophilac.discord.models.Channel;
import com.daophilac.discord.models.Message;
import com.daophilac.discord.models.User;
import com.daophilac.discord.resources.UiDecoration;
import com.daophilac.discord.resources.Route;
import com.daophilac.discord.tools.APICaller;
import com.daophilac.discord.tools.JsonBuilder;
import com.daophilac.discord.tools.JsonConverter;

import java.io.File;
import java.util.List;
import java.util.Locale;

public class MainActivity extends AppCompatActivity implements NavigatorListener {
    public static final String LOG_TAG = "com.daophilac.discord";
    public static Locale locale;
    private Inventory inventory;
    private LinearLayout linearLayoutMessage;
    private DrawerLayout drawerLayout;
    private NavigationView navigationView;
    private Toolbar toolbar;
    private ActionBar actionBar;
    private EditText editTextType;
    private ImageButton buttonSend;
    private FragmentManager fragmentManager;
    private NavigatorFragment navigatorFragment;
    public Handler backgroundHandler;
    private Thread threadBackground;
    private APICaller apiCaller;
    private JsonBuilder jsonBuilder;
    private JsonConverter jsonConverter;
    private String baseURL;

    private int messageTextColor;
    private int messageTextSize;

    private MainActivityListener listener;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        initializeGlobalVariable();
        String temp = this.getIntent().getStringExtra("jsonUser");
//        User tUser = this.jsonConverter.toUser()
        this.inventory.storeCurrentUser(this.getIntent().getStringExtra("jsonUser"));
        writeLogConsole(this.inventory.loadCurrentUser().getEmail());
        this.fragmentManager.beginTransaction().replace(R.id.navigation_view, navigatorFragment).commit();
    }

    private void initializeGlobalVariable() {
        locale = Locale.ENGLISH;// TODO:
        this.inventory = new Inventory();
        this.linearLayoutMessage = findViewById(R.id.linear_layout_message);
        this.drawerLayout = findViewById(R.id.drawer_layout);
        this.navigationView = findViewById(R.id.navigation_view);
        this.toolbar = findViewById(R.id.toolbar);
        this.editTextType = findViewById(R.id.editText_type);
        this.buttonSend = findViewById(R.id.button_send);
        setSupportActionBar(toolbar);
        this.actionBar = getSupportActionBar();
        this.actionBar.setDisplayHomeAsUpEnabled(true);
        this.actionBar.setHomeAsUpIndicator(R.drawable.ic_navigator);
        this.navigatorFragment = new NavigatorFragment();
        this.listener = this.navigatorFragment;
        this.listener.onCreateInventory(this.inventory);
        this.fragmentManager = getSupportFragmentManager();
        this.baseURL = Route.protocol + "://" + Route.serverIP + "/" + Route.serverName;
        this.apiCaller = new APICaller();
        this.jsonBuilder = new JsonBuilder();
        this.jsonConverter = new JsonConverter();
        this.messageTextColor = UiDecoration.messageTextColor;
        this.messageTextSize = UiDecoration.messageTextSize;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                drawerLayout.openDrawer(GravityCompat.START);
        }
        return super.onOptionsItemSelected(item);
    }
    public void sendMessage(View view){
        Channel currentChannel = this.inventory.loadCurrentChannel();
        final User currentUser = this.inventory.loadCurrentUser();
        String json = this.jsonBuilder.buildMessageJSON(currentChannel, currentUser, this.editTextType.getText().toString());
        this.backgroundHandler = new Handler(Looper.getMainLooper()){
            @Override
            public void handleMessage(android.os.Message msg) {
                super.handleMessage(msg);
                // TODO: check this one later, am I doing too much work here?
                Message message = jsonConverter.toMessage(msg.obj.toString());
                MessageTextView messageTextView = new MessageTextView(getBaseContext());
                messageTextView.setMessageID(message.getMessageId());
                messageTextView.setTextColor(messageTextColor);
                messageTextView.setTextSize(messageTextSize);
                messageTextView.setText(currentUser.getUserId() + ": " + editTextType.getText().toString());
                linearLayoutMessage.addView(messageTextView);
                editTextType.setText("");
            }
        };
        String requestURL = this.baseURL.concat(Route.urlInsertMessage);
        this.apiCaller.setProperties(this.backgroundHandler, "POST", requestURL, json);
        this.threadBackground = new Thread(this.apiCaller);
        this.threadBackground.start();
    }
    private void deleteDirectory(File fileOrDirectory) {
        if (fileOrDirectory.isDirectory()) {
            for (File child : fileOrDirectory.listFiles()) {
                deleteDirectory(child);
            }
        }
        fileOrDirectory.delete();
    }
    private void deleteAppData() {
        ContextWrapper contextWrapper = new ContextWrapper(this);
        File directory = new File(contextWrapper.getFilesDir() + "/" + getString(R.string.account_internal_directory));
        deleteDirectory(directory);
        Toast.makeText(this, "Deleted all data", Toast.LENGTH_LONG).show();
    }

    public static void writeLogConsole(String message) {
        Log.v(LOG_TAG, message);
    }

    @Override
    public void onChannelChanged(int channelID) {
        initializeMessageSection(channelID);
    }

    private void initializeMessageSection(int channelID) {
        if (this.linearLayoutMessage.getChildCount() > 0) {
            this.linearLayoutMessage.removeAllViews();
        }
        List<Message> listMessage = this.inventory.loadListMessage();
        Message message;
        // TODO: maybe I should add these messages into inventory as well?
        // TODO: remove message by id is a must feature, but it might be tricky
        MessageTextView messageTextView;
        for (int i = 0; i < listMessage.size(); i++) {
            message = listMessage.get(i);
            messageTextView = new MessageTextView(this);
            messageTextView.setMessageID(message.getMessageId());
            messageTextView.setTextColor(this.messageTextColor);
            messageTextView.setTextSize(this.messageTextSize);
            messageTextView.setText(message.getUserId() + ": " + message.getContent());
            this.linearLayoutMessage.addView(messageTextView);
        }
    }
}