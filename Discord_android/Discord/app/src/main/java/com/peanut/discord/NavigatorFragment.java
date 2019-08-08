package com.peanut.discord;
import android.content.Context;
import android.os.Bundle;
import android.os.Message;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CompoundButton;
import android.widget.ImageButton;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.widget.SwitchCompat;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.peanut.discord.customview.AddServerButton;
import com.peanut.discord.customview.ChannelRecyclerView;
import com.peanut.discord.customview.MoreButton;
import com.peanut.discord.customview.ServerRecyclerView;
import com.peanut.discord.customview.UserSettingButton;
import com.peanut.discord.equipment.Inventory;
import com.peanut.discord.interfaces.NavigatorListener;
import com.peanut.discord.models.Server;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;
public class NavigatorFragment extends Fragment {
    private NavigatorListener navigatorListener;
    private APICaller apiCaller;
    private ServerRecyclerView serverRecyclerView;
    private ChannelRecyclerView channelRecyclerView;
    private AddServerButton addServerButton;
    private MoreButton moreButton;
    private TextView textViewServerName;
    private ImageButton imageButtonAddChannel;
    private TextView textViewUserName;
    private UserSettingButton userSettingButton;

    private SwitchCompat switchCompatTest;
    private OnThemeChangeListener onThemeChangeListener;
    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        Inventory.registerOnUserSelectServerListener(server -> {
            textViewServerName.setText(Inventory.currentServer.getServerName());
            apiGetListChannel(server);
        });
        Inventory.registerOnUserSelectChannelListener(channel -> {
            if(Inventory.currentChannel != channel){
                this.navigatorListener.onChannelChanged(Inventory.currentChannel, channel);
                Inventory.currentChannel = channel;
            }
        });
        navigatorListener = (NavigatorListener) context;
        apiCaller = new APICaller();
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = MainActivity.themeInflater.inflate(R.layout.fragment_navigator, container, false);
        serverRecyclerView = view.findViewById(R.id.server_recycler_view);
        channelRecyclerView = view.findViewById(R.id.channel_recycler_view);
        addServerButton = view.findViewById(R.id.add_server_button);
        moreButton = view.findViewById(R.id.more_button);
        textViewServerName = view.findViewById(R.id.text_view_server_name);
        imageButtonAddChannel = view.findViewById(R.id.image_button_add_channel);
        textViewUserName = view.findViewById(R.id.text_view_user_name);
        userSettingButton = view.findViewById(R.id.user_setting_button);

        if(Inventory.currentServer != null){
            textViewServerName.setText(Inventory.currentServer.getServerName());
        }
        textViewUserName.setText(Inventory.currentUser.getUserName());
        serverRecyclerView.setAdapter(Inventory.getServerAdapter());
        serverRecyclerView.setLayoutManager(new LinearLayoutManager(getContext()));
        channelRecyclerView.setAdapter(Inventory.getChannelAdapter());
        channelRecyclerView.setLayoutManager(new LinearLayoutManager(getContext()));

        addServerButton.setOnClickListener(v -> navigatorListener.onAddOrCreateServer());
        imageButtonAddChannel.setOnClickListener(v -> {
            if(Inventory.currentServer.getAdminId() == Inventory.currentUser.getUserId()){
                new CreateChannelDialogFragment().show(getFragmentManager(), null);
            }
        });
        moreButton.setOnClickListener(v -> {
            new ServerConfigurationDialogFragment().show(getFragmentManager(), null);
        });
        userSettingButton.setOnClickListener(v -> {
            new LogOutConfirmDialogFragment().show(getFragmentManager(), null);
        });
        apiGetListServer();
        switchCompatTest = view.findViewById(R.id.test_switch);
        switchCompatTest.setChecked(MainActivity.themeId == R.style.DarkAMOLED);
        switchCompatTest.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if(onThemeChangeListener != null){
                    int themeId = R.style.Light;
                    if(isChecked){
                        themeId = R.style.DarkAMOLED;
                    }
                    onThemeChangeListener.onThemeChange(themeId);
                }
            }
        });
        return view;
    }

    private void apiGetListServer() {
        apiCaller.setProperties(APICaller.RequestMethod.GET, Route.Server.buildGetByUserUrl(Inventory.currentUser.getUserId()));
        apiCaller.setOnSuccessListener((connection, response) -> {
            getActivity().runOnUiThread(() -> Inventory.storeListServer(response));
        }).sendRequest();
    }

    private void apiGetListChannel(Server server) {
        apiCaller.setProperties(APICaller.RequestMethod.GET, Route.Channel.buildGetByServerUrl(server.getServerId()));
        apiCaller.setOnSuccessListener((connection, response) -> {
            getActivity().runOnUiThread(() -> Inventory.storeListChannel(response));
        }).sendRequest();
    }
    public void setOnThemeChangeListener(OnThemeChangeListener onThemeChangeListener) {
        this.onThemeChangeListener = onThemeChangeListener;
    }
    public interface OnThemeChangeListener{
        void onThemeChange(int themeId);
    }
}