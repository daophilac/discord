package com.daophilac.discord.recyclerviews;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.daophilac.discord.R;
import com.daophilac.discord.models.Channel;

import java.util.List;

import io.reactivex.annotations.NonNull;

public class ChannelAdapter extends RecyclerView.Adapter<ChannelAdapter.ViewHolder> {
    private List<Channel> listChannel;
    public ChannelAdapter(List<Channel> listChannel){
        this.listChannel = listChannel;
    }
    @android.support.annotation.NonNull
    @Override
    public ViewHolder onCreateViewHolder(@android.support.annotation.NonNull ViewGroup viewGroup, int i) {
        Context context = viewGroup.getContext();
        LayoutInflater layoutInflater = LayoutInflater.from(context);
        View viewHolder = layoutInflater.inflate(R.layout.channel_item, viewGroup, false);
        return new ViewHolder(viewHolder);
    }

    @Override
    public void onBindViewHolder(@android.support.annotation.NonNull ViewHolder viewHolder, int i) {
        Channel channel = this.listChannel.get(i);
        TextView textView = viewHolder.textView;
        textView.setText(channel.getName());
    }

    @Override
    public int getItemCount() {
        return this.listChannel.size();
    }

    class ViewHolder extends RecyclerView.ViewHolder{
        private TextView textView;
        ViewHolder(@NonNull View itemView){
            super(itemView);
            textView = itemView.findViewById(R.id.textView);
        }
    }
}
