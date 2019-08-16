package com.peanut.discord.equipment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;

import androidx.annotation.NonNull;
import androidx.fragment.app.FragmentManager;
import androidx.recyclerview.widget.RecyclerView;

import com.peanut.discord.CreateOrJoinServerDialogFragment;
import com.peanut.discord.MainActivity;
import com.peanut.discord.R;
import com.peanut.discord.customview.AddServerButton;
import com.peanut.discord.models.Server;

import java.util.ArrayList;
import java.util.List;

public class ServerAdapter extends RecyclerView.Adapter<ServerAdapter.ServerViewHolder> {
    private FragmentManager fragmentManager;
    private AddServerButton addServerButton;
    private CreateOrJoinServerDialogFragment createOrJoinServerDialogFragment;
    private List<OnServerButtonClickedListener> onServerButtonClickedListeners;
    private List<OnServerChangedListener> onServerChangedListeners;
    public ServerAdapter(FragmentManager fragmentManager){
        this.fragmentManager = fragmentManager;
    }
    public void establish(){
        createOrJoinServerDialogFragment = new CreateOrJoinServerDialogFragment();
        onServerButtonClickedListeners = new ArrayList<>();
        onServerChangedListeners = new ArrayList<>();


    }
    public void setAddServerButton(AddServerButton addServerButton) {
        this.addServerButton = addServerButton;
        addServerButton.setOnClickListener(v -> {
            createOrJoinServerDialogFragment.show(fragmentManager, MainActivity.LOG_TAG);
        });
    }
    public void tearDown(){
        onServerButtonClickedListeners = null;
        onServerChangedListeners = null;
    }

    @NonNull
    @Override
    public ServerViewHolder onCreateViewHolder(@NonNull ViewGroup viewGroup, int i) {
        LayoutInflater layoutInflater = LayoutInflater.from(viewGroup.getContext());
        View serverView = layoutInflater.inflate(R.layout.item_server, viewGroup, false);
        return new ServerViewHolder(serverView);
    }

    @Override
    public void onBindViewHolder(@NonNull ServerViewHolder serverViewHolder, int i) {
        Server server = Inventory.getServers().get(i);
        Button button = serverViewHolder.button;
        button.setText(server.getServerName());
        button.setOnClickListener(v -> {
            if(server != Inventory.currentServer){
                for(OnServerChangedListener listener : onServerChangedListeners){
                    listener.onServerChanged(Inventory.currentServer, server);
                }
            }
        });
    }
    @Override
    public int getItemCount() {
        if(Inventory.getServers() == null){
            return 0;
        }
        return Inventory.getServers().size();
    }

    class ServerViewHolder extends RecyclerView.ViewHolder{
        private Button button;//TODO
        private ServerViewHolder(@NonNull View itemView) {
            super(itemView);
            this.button = itemView.findViewById(R.id.button_server);
        }
    }
    boolean registerOnServerButtonClickedListener(OnServerButtonClickedListener listener){
        if(listener == null){
            return false;
        }
        return onServerButtonClickedListeners.add(listener);
    }
    boolean registerOnServerChangedListener(OnServerChangedListener listener){
        if(listener == null){
            return false;
        }
        return onServerChangedListeners.add(listener);
    }
    boolean unregisterOnServerButtonClickedListener(OnServerButtonClickedListener listener){
        if(listener == null){
            return false;
        }
        return onServerButtonClickedListeners.remove(listener);
    }
    boolean unregisterOnServerChangedListener(OnServerChangedListener listener){
        if(listener == null){
            return false;
        }
        return onServerChangedListeners.remove(listener);
    }
    interface OnServerButtonClickedListener{
        void onServerButtonClicked(Server server);
    }
    interface OnServerChangedListener{
        void onServerChanged(Server previous, Server now);
    }
}
