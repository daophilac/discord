package com.peanut.discord;

import android.content.Context;
import android.content.ContextWrapper;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.transition.Slide;
import android.support.v4.app.FragmentManager;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.view.ContextThemeWrapper;
import android.support.v7.widget.LinearLayoutManager;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.inputmethod.InputMethodManager;
import android.widget.EditText;
import android.widget.Toast;

import com.peanut.discord.customview.MessageRecyclerView;
import com.peanut.discord.customview.NavigationButton;
import com.peanut.discord.customview.SendMessageButton;
import com.peanut.discord.equipment.HubManager;
import com.peanut.discord.equipment.Inventory;
import com.peanut.discord.interfaces.NavigatorListener;
import com.peanut.discord.interfaces.ServerConfigurationListener;
import com.peanut.discord.models.Channel;
import com.peanut.discord.models.Server;
import com.peanut.discord.models.User;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;
import com.peanut.discord.tools.JsonBuilder;
import com.peanut.discord.tools.JsonConverter;
import com.peanut.discord.worker.MultipleWorker;
import com.peanut.discord.worker.SingleWorker;

import java.io.File;
import java.util.Locale;

public class MainActivity extends AppCompatActivity implements HubManager.HubListener, NavigatorListener, ServerConfigurationListener {
    public static final String LOG_TAG = "com.peanut.discord";
    public static Locale locale = Locale.ENGLISH;// TODO:
    public static LayoutInflater themeInflater;// TODO
    public static final int themeId = R.style.AMOLED_dark_theme;

    public static InputMethodManager inputMethodManager;
    public static JsonBuilder jsonBuilder;
    public static JsonConverter jsonConverter;

    private DrawerLayout drawerLayout;
    private NavigationButton navigationButton;
    private EditText editTextType;
    private SendMessageButton sendMessageButton;

    private FragmentManager fragmentManager;
    private NavigatorFragment navigatorFragment;
    private MessageRecyclerView messageRecyclerView;
    private Handler handlerGetListMessage;
    private Handler handlerLeaveServer;
    private APICaller apiCaller;

    private SingleWorker singleWorker;
    private MultipleWorker multipleWorker;



    @Override
    protected void onDestroy() {
        super.onDestroy();
        singleWorker.quit();
        multipleWorker.quit();
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        themeInflater = getLayoutInflater().cloneInContext(new ContextThemeWrapper(this, themeId));
        setTheme(themeId);
        setContentView(R.layout.activity_main);
        HubManager.establish();
        HubManager.registerListener(this);
        Inventory.prepare();
        Inventory.storeCurrentUser(getIntent().getStringExtra("jsonUser"));
        singleWorker = new SingleWorker(LOG_TAG);
        multipleWorker = new MultipleWorker(LOG_TAG, 3);
        fragmentManager = getSupportFragmentManager();
        navigatorFragment = new NavigatorFragment();
        apiCaller = new APICaller();
        singleWorker.execute(() -> {
            handlerGetListMessage = new Handler(Looper.getMainLooper()){
                @Override
                public void handleMessage(Message msg) {
                    Inventory.storeListMessage((String)msg.obj);
                }
            };
            handlerLeaveServer = new Handler(Looper.getMainLooper()){
                @Override
                public void handleMessage(Message msg) {

                }
            };
            navigatorFragment.setEnterTransition(new Slide(Gravity.START));
            navigatorFragment.setExitTransition(new Slide(Gravity.START));
        });
        multipleWorker.execute(() -> {
            inputMethodManager = (InputMethodManager) getBaseContext().getSystemService(Context.INPUT_METHOD_SERVICE);
            jsonBuilder = new JsonBuilder();
            jsonConverter = new JsonConverter();
        }).execute(() -> {
            drawerLayout = findViewById(R.id.drawer_layout);
            navigationButton = findViewById(R.id.navigation_button);
            messageRecyclerView = findViewById(R.id.message_recycler_view);
            editTextType = findViewById(R.id.editText_type);
            sendMessageButton = findViewById(R.id.send_message_button);

            navigationButton.setOnClickListener(v -> drawerLayout.openDrawer(GravityCompat.START));
            sendMessageButton.setOnClickListener(v -> {
                if(!editTextType.getText().toString().equals("")){
                    sendMessage();
                }
            });
            runOnUiThread(() -> {
                messageRecyclerView.setAdapter(Inventory.getMessageAdapter());
                messageRecyclerView.setLayoutManager(new LinearLayoutManager(this));
                fragmentManager.beginTransaction().replace(R.id.navigation_view, navigatorFragment).commit();
            });
        });
    }
    private void apiGetListMessage(Channel channel){
        apiCaller.setProperties(handlerGetListMessage, APICaller.RequestMethod.GET, Route.buildGetMessagesByChannelUrl(channel.getChannelId()));
        singleWorker.execute(apiCaller);
    }

    public void sendMessage(){
        User currentUser = Inventory.currentUser;
        Channel currentChannel = Inventory.currentChannel;
        String json = jsonBuilder.buildMessageJson(currentChannel, currentUser, editTextType.getText().toString());
        HubManager.sendMessage(currentUser.getUserId(), currentChannel.getChannelId(), json);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                drawerLayout.openDrawer(GravityCompat.START);
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onAddOrCreateServer() {
        new CreateOrJoinServerDialogFragment().show(fragmentManager, null);
    }

    @Override
    public void onChannelChanged(Channel previousChannel, Channel currentChannel) {
        if(previousChannel != null){
            HubManager.leaveChannel(previousChannel.getChannelId());
        }
        HubManager.joinChannel(currentChannel.getChannelId());
        apiGetListMessage(currentChannel);
    }

    @Override
    public void onLeaveServer() {
        apiCaller.setProperties(handlerLeaveServer, APICaller.RequestMethod.DELETE, Route.buildLeaveServerUrl(Inventory.currentUser.getUserId(), Inventory.currentServer.getServerId()));
        singleWorker.execute(apiCaller);
        Inventory.leaveServer();
    }

    @Override
    public void onReceiveMessage(String connectionId, int userId, String jsonMessage) {
        Inventory.addMessage(jsonMessage);
        if(HubManager.connectionId.equals(connectionId)){
            runOnUiThread(() -> editTextType.setText(""));
        }
    }

    SingleWorker getSingleWorker() {
        return singleWorker;
    }

    MultipleWorker getMultipleWorker() {
        return multipleWorker;
    }

    public static void writeLogVerbose(String message) {
        Log.v(LOG_TAG, message);
    }


    @Override
    protected void onNewIntent(Intent intent) {
        IntentCommand intentCommand = (IntentCommand) intent.getSerializableExtra("command");
        switch (intentCommand){
            case CREATE_SERVER:
                Inventory.addServer(intent.getStringExtra("jsonServer"));
                break;
            case JOIN_SERVER:
                Server newServer = jsonConverter.toServer(intent.getStringExtra("jsonServer"));
                if(!Inventory.loadListServer().contains(newServer)){
                    Inventory.addServer(newServer);
                }
                break;
        }
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

    enum IntentCommand{
        CREATE_SERVER, JOIN_SERVER
    }
}