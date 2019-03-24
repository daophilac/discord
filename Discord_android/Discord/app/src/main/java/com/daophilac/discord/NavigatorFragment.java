package com.daophilac.discord;

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
import android.widget.Button;
import android.widget.LinearLayout;

import com.daophilac.discord.customview.ServerButton;
import com.daophilac.discord.models.Server;

import java.util.List;

public class NavigatorFragment extends Fragment {
    public Handler backgroundHandler;
    private Thread threadBackground;
    private View view;
    private LinearLayout linearLayoutServer;
    private LinearLayout linearLayoutChannel;
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


        return this.view;
    }
    public void initializeGlobalVariable(){
        this.linearLayoutServer = this.view.findViewById(R.id.linear_layout_server);
        this.linearLayoutChannel = this.view.findViewById(R.id.linear_layout_channel);
    }
    public void initializeServerSection(List<Server> listServer){// TODO:
        ServerButton serverButton;
        for(int i = 0; i < listServer.size(); i++){
            serverButton = new ServerButton(this.getContext());
            serverButton.setText(listServer.get(i).getName());
            serverButton.setServerID(listServer.get(i).getServerID());
            this.linearLayoutServer.addView(serverButton);
        }
    }
    public void handleChannelSection(){

    }
    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

    }

    @Override
    public void onResume() {
        super.onResume();
    }
}
