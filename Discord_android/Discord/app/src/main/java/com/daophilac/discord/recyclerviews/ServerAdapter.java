package com.daophilac.discord.recyclerviews;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;

import com.daophilac.discord.NavigatorFragment;
import com.daophilac.discord.R;
import com.daophilac.discord.models.Server;

import java.util.List;

public class ServerAdapter extends RecyclerView.Adapter<ServerAdapter.ViewHolder>{
    private NavigatorFragment navigatorFragment;
    private List<Server> listServer;
    private Listener listener;
    public ServerAdapter(List<Server> listServer, Listener listener){
        this.listServer = listServer;
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup viewGroup, int i) {
        Context context = viewGroup.getContext();
        LayoutInflater layoutInflater = LayoutInflater.from(context);
        View serverView = layoutInflater.inflate(R.layout.server_item, viewGroup, false);
//        this.listener = ((NavigatorFragment)context).getListener();
        return new ViewHolder(serverView);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder viewHolder, int i) {
        Server server = this.listServer.get(i);
        viewHolder.serverId = server.getServerId();
        Button button = viewHolder.button;
        button.setText(server.getName());
        this.listener.onButtonClick(viewHolder);
//        button.setOnClickListener(new );
    }
    @Override
    public int getItemCount() {
        return this.listServer.size();
    }
    public class ViewHolder extends RecyclerView.ViewHolder{
        private int serverId;
        private ImageView imageView;
        private Button button;//TODO
        ViewHolder(@NonNull View itemView) {
            super(itemView);
            this.button = itemView.findViewById(R.id.buttonServer);
        }

        public int getServerId() {
            return serverId;
        }

        public ImageView getImageView() {
            return imageView;
        }

        public Button getButton() {
            return button;
        }
    }
    public interface Listener {
        static NavigatorFragment na = null;
        void onButtonClick(ViewHolder viewHolder);

    }
}
