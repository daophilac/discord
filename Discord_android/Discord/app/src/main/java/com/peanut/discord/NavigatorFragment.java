package com.peanut.discord;

import android.content.Context;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.TextView;

import com.peanut.discord.customview.AddServerButton;
import com.peanut.discord.customview.ChannelRecyclerView;
import com.peanut.discord.customview.MoreButton;
import com.peanut.discord.customview.ServerRecyclerView;
import com.peanut.discord.customview.UserSettingButton;
import com.peanut.discord.equipment.Inventory;
import com.peanut.discord.interfaces.NavigatorListener;
import com.peanut.discord.models.Channel;
import com.peanut.discord.models.Server;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;
import com.peanut.discord.worker.SingleWorker;
public class NavigatorFragment extends Fragment implements Inventory.InventoryListener {
    private static boolean registeredListener = false;
    private NavigatorListener navigatorListener;
    private APICaller apiCaller;
    private Handler handlerServer;
    private Handler handlerChannel;
    private ServerRecyclerView serverRecyclerView;
    private ChannelRecyclerView channelRecyclerView;
    private AddServerButton addServerButton;
    private MoreButton moreButton;
    private TextView textViewServerName;
    private ImageButton imageButtonAddChannel;
    private TextView textViewUserName;
    private UserSettingButton userSettingButton;

    private SingleWorker singleWorker;
    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        Inventory.registerListener(this);
        this.navigatorListener = (NavigatorListener) context;
        this.singleWorker = ((MainActivity)context).getSingleWorker();
        this.apiCaller = new APICaller();
        this.handlerServer = new Handler(Looper.getMainLooper()){
            @Override
            public void handleMessage(Message msg) {
                Inventory.storeListServer(msg.obj.toString());
            }
        };
        this.handlerChannel = new Handler(Looper.getMainLooper()){
            @Override
            public void handleMessage(Message msg) {
                Inventory.storeListChannel(msg.obj.toString());
            }
        };
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = MainActivity.themeInflater.inflate(R.layout.fragment_navigator, container, false);
        this.serverRecyclerView = view.findViewById(R.id.server_recycler_view);
        this.channelRecyclerView = view.findViewById(R.id.channel_recycler_view);
        this.addServerButton = view.findViewById(R.id.add_server_button);
        this.moreButton = view.findViewById(R.id.more_button);
        this.textViewServerName = view.findViewById(R.id.text_view_server_name);
        this.imageButtonAddChannel = view.findViewById(R.id.image_button_add_channel);
        this.textViewUserName = view.findViewById(R.id.text_view_user_name);
        this.userSettingButton = view.findViewById(R.id.user_setting_button);

        this.textViewUserName.setText(Inventory.currentUser.getUserName());
        this.serverRecyclerView.setAdapter(Inventory.getServerAdapter());
        this.serverRecyclerView.setLayoutManager(new LinearLayoutManager(getContext()));
        this.channelRecyclerView.setAdapter(Inventory.getChannelAdapter());
        this.channelRecyclerView.setLayoutManager(new LinearLayoutManager(getContext()));

        this.addServerButton.setOnClickListener(v -> this.navigatorListener.onAddOrCreateServer());
        this.imageButtonAddChannel.setOnClickListener(v -> {
            if(Inventory.currentServer.getAdminId() == Inventory.currentUser.getUserId()){
                new CreateChannelDialogFragment().show(getFragmentManager(), null);
            }
        });
        this.moreButton.setOnClickListener(v -> {
            new ServerConfigurationDialogFragment().show(getFragmentManager(), null);
        });
        this.userSettingButton.setOnClickListener(v -> {
            new LogOutConfirmDialogFragment().show(getFragmentManager(), null);
        });
        apiGetListServer();
        return view;
    }

    private void apiGetListServer() {
        this.apiCaller.setProperties(this.handlerServer, APICaller.RequestMethod.GET, Route.buildGetSeversByUserUrl(Inventory.currentUser.getUserId()));
        this.singleWorker.execute(this.apiCaller);
    }

    private void apiGetListChannel(Server server) {
        this.apiCaller.setProperties(this.handlerChannel, APICaller.RequestMethod.GET, Route.buildGetChannelsByServerUrl(server.getServerId()));
        this.singleWorker.execute(this.apiCaller);
    }
    @Override
    public void onSelectServer(Server server) {
        this.textViewServerName.setText(Inventory.currentServer.getName());
        apiGetListChannel(server);
    }

    @Override
    public void onSelectChannel(Channel channel) {
        if(Inventory.currentChannel != channel){
            this.navigatorListener.onChannelChanged(Inventory.currentChannel, channel);
            Inventory.currentChannel = channel;
        }
    }
}