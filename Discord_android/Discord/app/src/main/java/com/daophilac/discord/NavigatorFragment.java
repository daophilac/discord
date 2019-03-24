package com.daophilac.discord;

import android.graphics.Color;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;

import com.daophilac.discord.customview.ChannelTextView;
import com.daophilac.discord.customview.ServerButton;
import com.daophilac.discord.models.Channel;
import com.daophilac.discord.models.Server;

import java.util.List;

public class NavigatorFragment extends Fragment {
    private View view;
    private Handler backgroundHandler;
    private Thread threadBackground;
    private APICaller apiCaller;
    private String baseURL;
    private Inventory inventory;
    private LinearLayout linearLayoutServer;
    private LinearLayout linearLayoutChannel;
    private int channelTextColor;
    private int channelTextSize;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        this.view = inflater.inflate(R.layout.fragment_navigator, container, false);
        initializeGlobalVariable();
//        LinearLayout linearLayoutServer = view.findViewById(R.id.linear_layout_server);
//        this.backgroundHandler = new Handler(Looper.getMainLooper()){
//            @Override
//            public void handleMessage(Message msg) {
//                super.handleMessage(msg);
//                handleJSON(msg.obj.toString());
//            }
//        };
//        this.apiCaller = new APICaller(this.backgroundHandler);
//
//        this.threadBackground = new Thread(this.apiCaller);
//        this.threadBackground.start();
//        ImageButton imageButton = view.findViewById(R.id.image_button);
//        String path = "/sdcard/server_1.png";
//        Bitmap bitmap = BitmapFactory.decodeFile(path);
//        imageButton.setBackground(new BitmapDrawable(getResources(), bitmap));

//        String path = "/sdcard/server_1.png";
//        Bitmap bitmap = BitmapFactory.decodeFile(path);
        //Button button = view.findViewById(R.id.button);
        //button.setTag();
        //button.setText("Final Fantasy");
        //Button button2 = view.findViewById(R.id.button2);
        //button.setText("Zwei The Ark of Naphistism");
//        button.setBackground(new BitmapDrawable(getResources(), bitmap));


        loadListServer();
        return this.view;
    }

    public void setInventory(Inventory inventory) {
        this.inventory = inventory;
    }

    public void initializeGlobalVariable() {
        this.linearLayoutServer = this.view.findViewById(R.id.linear_layout_server);
        this.linearLayoutChannel = this.view.findViewById(R.id.linear_layout_channel);
        this.channelTextColor = Color.WHITE;
        this.channelTextSize = 20;
        this.baseURL = "http://" + Route.serverIP + "/" + Route.serverName;
        this.apiCaller = new APICaller();
    }

    public void initializeServerSection(List<Server> listServer) {
        ServerButton serverButton;
        for (int i = 0; i < listServer.size(); i++) {
            serverButton = new ServerButton(this.getContext());
            serverButton.setServerID(listServer.get(i).getServerID());
            serverButton.setText(listServer.get(i).getName());
            serverButton.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    loadListChannel(((ServerButton) v).getServerID());
                }
            });
            this.linearLayoutServer.addView(serverButton);
        }
    }

    public void initializeChannelSection(List<Channel> listChannel) {
        if (this.linearLayoutChannel.getChildCount() > 0) {
            this.linearLayoutChannel.removeAllViews();
        }
        ChannelTextView channelTextView;
        for (int i = 0; i < listChannel.size(); i++) {
            channelTextView = new ChannelTextView(this.getContext());
            channelTextView.setChannelID(listChannel.get(i).getChannelID());
            channelTextView.setText(String.format(MainActivity.locale, InterfaceFormation.channelName, i, listChannel.get(i).getName()));
            channelTextView.setTextColor(this.channelTextColor);
            channelTextView.setTextSize(this.channelTextSize);
            this.linearLayoutChannel.addView(channelTextView);
        }
    }

    private void loadListChannel(int serverID) {
        this.backgroundHandler = new Handler(Looper.getMainLooper()) {
            @Override
            public void handleMessage(Message msg) {
                super.handleMessage(msg);
                String json = msg.obj.toString();
                inventory.storeListChannel(msg.obj.toString());
                initializeChannelSection(inventory.loadListChannel());
            }
        };
        this.apiCaller.setHandler(this.backgroundHandler);
        this.apiCaller.setRequestMethod("GET");
        String requestURL = this.baseURL.concat(String.format(MainActivity.locale, Route.urlGetChannelsByServer, serverID));
        this.apiCaller.setRequestURL(requestURL);
        this.threadBackground = new Thread(this.apiCaller);
        this.threadBackground.start();
    }

    private void loadListServer() {
        this.backgroundHandler = new Handler(Looper.getMainLooper()) {
            @Override
            public void handleMessage(Message msg) {
                super.handleMessage(msg);
                String json = msg.obj.toString();
                inventory.storeListServer(msg.obj.toString());
                initializeServerSection(inventory.loadListServer());
            }
        };
        this.apiCaller.setHandler(this.backgroundHandler);
        this.apiCaller.setRequestMethod("GET");
        String requestURL = this.baseURL.concat(String.format(MainActivity.locale, Route.urlGetServersByUser, this.inventory.loadUser().getUserID()));
        this.apiCaller.setRequestURL(requestURL);
        this.threadBackground = new Thread(this.apiCaller);
        this.threadBackground.start();
    }
}