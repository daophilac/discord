package com.peanut.discord.equipment;

import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;

import com.peanut.discord.R;
import com.peanut.discord.models.Server;

import java.util.ArrayList;
import java.util.List;

class ServerAdapter extends RecyclerView.Adapter<ServerAdapter.ServerViewHolder> implements Inventory.ServerListener {
    private List<Server> listServer;
    private ServerAdapterListener serverAdapterListener;
    private Handler handler;
    ServerAdapter(ServerAdapterListener serverAdapterListener){
        this.listServer = new ArrayList<>();
        this.serverAdapterListener = serverAdapterListener;
        this.handler = new Handler(Looper.getMainLooper()){
            @Override
            public void handleMessage(Message msg) {
                notifyItemInserted(listServer.size() - 1);
            }
        };
    }

    @Override
    public void onAddListServer(List<Server> listServer) {
        this.listServer = listServer;
        notifyDataSetChanged();
    }

    @Override
    public void onAddServer(Server server) {
        this.listServer.add(server);
        handler.sendEmptyMessage(0);
    }

    @Override
    public void onLeaveServer(Server server) {
        this.listServer.remove(server);
        notifyItemRemoved(this.listServer.size());
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
        Server server = this.listServer.get(i);
        Button button = serverViewHolder.button;
        button.setText(server.getName());
        button.setOnClickListener(v -> this.serverAdapterListener.onSelectServer(server));
    }
    @Override
    public int getItemCount() {
        return this.listServer.size();
    }

    List<Server> getListServer() {
        return listServer;
    }

    class ServerViewHolder extends RecyclerView.ViewHolder{
        private Button button;//TODO
        private ServerViewHolder(@NonNull View itemView) {
            super(itemView);
            this.button = itemView.findViewById(R.id.button_server);
        }
    }
}
