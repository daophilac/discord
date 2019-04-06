package com.daophilac.discord;

import android.content.Context;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;


import com.daophilac.discord.interfaces.MainActivityListener;
import com.daophilac.discord.interfaces.NavigatorListener;
import com.daophilac.discord.models.Server;
import com.daophilac.discord.models.Channel;
import com.daophilac.discord.recyclerviews.ChannelAdapter;
import com.daophilac.discord.recyclerviews.ServerAdapter;
import com.daophilac.discord.resources.UiDecoration;
import com.daophilac.discord.resources.Route;
import com.daophilac.discord.tools.APICaller;

import java.util.List;

public class NavigatorFragment extends Fragment implements MainActivityListener, ServerAdapter.Listener {
    private NavigatorListener listener;
    private View view;
    private Handler backgroundHandler;
    private Thread threadBackground;
    private APICaller apiCaller;
    private String baseUrl;
    private Inventory inventory;
    //private LinearLayout linearLayoutServer;
    //private LinearLayout linearLayoutChannel;
    private LinearLayout linearLayoutMessage;
    private RecyclerView recyclerViewServer;
    private RecyclerView recyclerViewChannel;
    private ServerAdapter serverAdapter;
    private ChannelAdapter channelAdapter;
    private ServerAdapter.Listener serverListener;
    private int currentSelectedChannel;
    private int channelTextColor;
    private int channelTextSize;
    private int messageTextColor;
    private int messageTextSize;

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        /* Assign the activity to the variable. Because activity implements OnFragmentInteractionListener
           so the activity is an instance of OnFragmentInteractionListener. */
        if (context instanceof NavigatorListener) {
            listener = (NavigatorListener) context;
            serverListener = (ServerAdapter.Listener) context;
        }
        else {
            throw new RuntimeException(context.toString() + " must implement OnNavigatorInteractionListener");
        }
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        this.view = inflater.inflate(R.layout.fragment_navigator, container, false);
        initializeGlobalVariable();
        loadListServer();
        return this.view;
    }

    public void initializeGlobalVariable() {
        //this.linearLayoutServer = this.view.findViewById(R.id.linear_layout_server);
        //this.linearLayoutChannel = this.view.findViewById(R.id.linear_layout_channel);
        this.recyclerViewServer = this.view.findViewById(R.id.recyclerView_server);
        this.recyclerViewChannel = this.view.findViewById(R.id.recyclerView_channel);
        this.linearLayoutMessage = this.view.findViewById(R.id.linear_layout_message);
        this.channelTextColor = UiDecoration.channelTextColor;
        this.channelTextSize = UiDecoration.channelTextSize;
        this.messageTextColor = UiDecoration.messageTextColor;
        this.messageTextSize = UiDecoration.messageTextSize;
        this.baseUrl = Route.buildBaseUrl();
        this.apiCaller = new APICaller();

//        this.serverAdapter = new ServerAdapter()
    }

    private void initializeServerSection(List<Server> listServer) {
        this.serverAdapter = new ServerAdapter(listServer, this.serverListener);
        this.recyclerViewServer.setAdapter(this.serverAdapter);
        this.recyclerViewServer.setLayoutManager(new LinearLayoutManager(this.getContext()));

//        ServerButton serverButton;
//        for (int i = 0; i < listServer.size(); i++) {
//            serverButton = new ServerButton(this.getContext());
//            serverButton.setServerID(listServer.get(i).getServerId());
//            serverButton.setText(listServer.get(i).getName());
//            serverButton.setOnClickListener(new View.OnClickListener() {
//                @Override
//                public void onClick(View v) {
//                    loadListChannel(((ServerButton) v).getServerID());
//                }
//            });
//            this.linearLayoutServer.addView(serverButton);
//        }
    }

    private void initializeChannelSection(List<Channel> listChannel) {
        if(this.channelAdapter != null){
            this.recyclerViewChannel.removeAllViews();
        }
        this.channelAdapter = new ChannelAdapter(listChannel);
        this.recyclerViewChannel.setAdapter(this.channelAdapter);
        this.recyclerViewChannel.setLayoutManager(new LinearLayoutManager(this.getContext()));
        this.channelAdapter.notifyDataSetChanged();

//        if (this.linearLayoutChannel.getChildCount() > 0) {
//            this.linearLayoutChannel.removeAllViews();
//        }
//        ChannelTextView channelTextView;
//        for (int i = 0; i < listChannel.size(); i++) {
//            channelTextView = new ChannelTextView(this.getContext());
//            channelTextView.setChannelID(listChannel.get(i).getChannelId());
//            channelTextView.setText(String.format(MainActivity.locale, UiDecoration.channelName, i, listChannel.get(i).getName()));
//            channelTextView.setTextColor(this.channelTextColor);
//            channelTextView.setTextSize(this.channelTextSize);
//            channelTextView.setOnClickListener(new View.OnClickListener() {
//                @Override
//                public void onClick(View v) {
//                    loadListMessage(((ChannelTextView) v).getChannelID());
//                }
//            });
//            this.linearLayoutChannel.addView(channelTextView);
//        }
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
        String requestURL = this.baseUrl.concat(String.format(MainActivity.locale, Route.urlGetServersByUser, this.inventory.loadCurrentUser().getUserId()));
        this.apiCaller.setRequestURL(requestURL);
        this.threadBackground = new Thread(this.apiCaller);
        this.threadBackground.start();
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
        String requestURL = this.baseUrl.concat(String.format(MainActivity.locale, Route.urlGetChannelsByServer, serverID));
        this.apiCaller.setRequestURL(requestURL);
        this.threadBackground = new Thread(this.apiCaller);
        this.threadBackground.start();
    }

    private void loadListMessage(final int channelID) {
        if (this.currentSelectedChannel == channelID) {
            return;
        }
        else{
            this.currentSelectedChannel = channelID;
            List<Channel> listChannel = this.inventory.loadListChannel();
            for(int i = 0; i < listChannel.size(); i++){
                if(listChannel.get(i).getChannelId() == channelID){
                    this.inventory.storeCurrentChannel(listChannel.get(i));
                    break;
                }
            }
        }
        this.backgroundHandler = new Handler(Looper.getMainLooper()) {
            @Override
            public void handleMessage(Message msg) {
                super.handleMessage(msg);
                String json = msg.obj.toString();
                inventory.storeListMessage(msg.obj.toString());
                listener.onChannelChanged(channelID);
            }
        };
        this.apiCaller.setHandler(this.backgroundHandler);
        this.apiCaller.setRequestMethod("GET");
        String requestURL = this.baseUrl.concat(String.format(MainActivity.locale, Route.urlGetMessagesByChannel, channelID));
        this.apiCaller.setRequestURL(requestURL);
        this.threadBackground = new Thread(this.apiCaller);
        this.threadBackground.start();
    }

    @Override
    public void onCreateInventory(Inventory inventory) {
        this.inventory = inventory;
    }

    @Override
    public void onButtonClick(ServerAdapter.ViewHolder viewHolder) {
        loadListChannel(viewHolder.getServerId());
    }
//    public ServerAdapter.Listener getListener(){
//        //return this.serverListener;
//    }
}