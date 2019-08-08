package com.peanut.discord;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Message;

import androidx.appcompat.view.ContextThemeWrapper;
import androidx.transition.Slide;
import androidx.fragment.app.FragmentManager;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.inputmethod.InputMethodManager;
import android.widget.EditText;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.peanut.androidlib.common.worker.MultipleWorker;
import com.peanut.androidlib.common.worker.SingleWorker;
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

import java.io.IOException;
import java.util.Locale;

public class MainActivity extends AppCompatActivity implements HubManager.HubListener, NavigatorListener, ServerConfigurationListener {
    public static final String preferenceName = "com.peanut.discord";
    public static final String LOG_TAG = "com.peanut.discord";
    public static final String dateTimePattern = "yyyy-MM-dd'T'HH:mm:ss.SSSSSSS";
    public static final Gson gson = new GsonBuilder().setDateFormat(dateTimePattern).create();
    public static Locale locale = Locale.ENGLISH;// TODO:
    public static LayoutInflater themeInflater;// TODO
    public static int themeId;
    public static SharedPreferences sharedPreferences;

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
    private APICaller apiCaller;

    private SingleWorker singleWorker;
    private MultipleWorker multipleWorker;

    @Override
    protected void onDestroy() {
        super.onDestroy();
        singleWorker.quit();
        multipleWorker.quit();
    }
    private void initialize(){
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
        Inventory.registerOnUserLongClickMessageListener(message -> {
            MessageOperationDialog mod = new MessageOperationDialog();
            mod.show(fragmentManager, getPackageName());
        });
        multipleWorker.execute(() -> {
            inputMethodManager = (InputMethodManager) getBaseContext().getSystemService(Context.INPUT_METHOD_SERVICE);
            jsonBuilder = new JsonBuilder();
            jsonConverter = new JsonConverter();
        }).execute(() -> {
            registerViews();
            runOnUiThread(() -> {
                navigatorFragment.setEnterTransition(new Slide(Gravity.START));
                navigatorFragment.setExitTransition(new Slide(Gravity.START));
                messageRecyclerView.setAdapter(Inventory.getMessageAdapter());
                messageRecyclerView.setLayoutManager(new LinearLayoutManager(this));
                fragmentManager.beginTransaction().replace(R.id.navigation_view, navigatorFragment).commit();
            });
            navigatorFragment.setOnThemeChangeListener(themeId -> {
                SharedPreferences.Editor editor = sharedPreferences.edit();
                editor.putInt("themeId", themeId);
                editor.apply();
                Intent i = new Intent(this, MainActivity.class);
                i.putExtra("command", IntentCommand.CHANGE_THEME);
                startActivity(i);
            });
        });
    }
    private void changeTheme(){
        themeId = sharedPreferences.getInt("themeId", R.style.Light);
        themeInflater = getLayoutInflater().cloneInContext(new ContextThemeWrapper(this, themeId));
        setTheme(themeId);
        setContentView(R.layout.activity_main);
        multipleWorker.execute(() -> {
            registerViews();
            runOnUiThread(() -> {
                messageRecyclerView.setAdapter(Inventory.getMessageAdapter());
                messageRecyclerView.setLayoutManager(new LinearLayoutManager(this));
                fragmentManager.beginTransaction().detach(navigatorFragment).attach(navigatorFragment).commit();
                drawerLayout.openDrawer(GravityCompat.START, false);
            });
        });
    }
    public void registerViews(){
        drawerLayout = findViewById(R.id.drawer_layout);
        navigationButton = findViewById(R.id.navigation_button);
        messageRecyclerView = findViewById(R.id.message_recycler_view);
        editTextType = findViewById(R.id.editText_type);
        sendMessageButton = findViewById(R.id.send_message_button);
        navigationButton.setOnClickListener(v -> drawerLayout.openDrawer(GravityCompat.START));
        sendMessageButton.setOnClickListener(v -> {
            if(Inventory.currentChannel == null){
                return;
            }
            if(!editTextType.getText().toString().equals("")){
                sendMessage();
            }
        });
    }
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        initialize();
    }
    private void apiGetListMessage(Channel channel){
        apiCaller.setProperties(APICaller.RequestMethod.GET, Route.Message.buildGetByChannelUrl(channel.getChannelId()));
        apiCaller.setOnSuccessListener((connection, response) -> {
            runOnUiThread(() -> Inventory.storeListMessage(response));
        }).sendRequest();
    }
    public void sendMessage(){
        User currentUser = Inventory.currentUser;
        Channel currentChannel = Inventory.currentChannel;
        String content = editTextType.getText().toString();
        HubManager.sendMessage(currentUser.getUserId(), currentChannel.getChannelId(), content);
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
            HubManager.exitChannel(previousChannel.getChannelId());
        }
        HubManager.enterChannel(currentChannel.getChannelId());
        apiGetListMessage(currentChannel);
    }

    @Override
    public void onGetNewChannel(String jsonChannel) {
        Inventory.addChannel(jsonChannel);
    }

    @Override
    public void onLeaveServer() {
        if(Inventory.currentServer == null){
            return;
        }
        apiCaller.setProperties(APICaller.RequestMethod.DELETE, Route.ServerUser.buildLeaveServerUrl(Inventory.currentUser.getUserId(), Inventory.currentServer.getServerId()));
        apiCaller.sendRequest();
        Inventory.leaveServer();
    }

    @Override
    public void onReceiveMessage(String connectionId, int userId, String jsonMessage) {
        Inventory.addMessage(jsonMessage);
        if(HubManager.connectionId.equals(connectionId)){
            runOnUiThread(() -> {
                editTextType.setText("");
            });
        }
    }
    public static void writeLogVerbose(String message) {
        Log.v(LOG_TAG, message);
    }
    public static void writeLogVerbose(int number) {
        Log.v(LOG_TAG, String.valueOf(number));
    }
    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        IntentCommand intentCommand = (IntentCommand) intent.getSerializableExtra("command");
        if(intentCommand == null){
            return;
        }
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
            case CHANGE_THEME:
                changeTheme();
                break;
        }
    }
    enum IntentCommand{
        CREATE_SERVER, JOIN_SERVER, CHANGE_THEME
    }
}