package com.peanut.discord.customview;

import android.content.Context;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.recyclerview.widget.RecyclerView;
import android.util.AttributeSet;

import com.peanut.discord.R;

public class ServerRecyclerView extends RecyclerView {
    public ServerRecyclerView(@NonNull Context context) {
        super(context);
    }
    public ServerRecyclerView(@NonNull Context context, @Nullable AttributeSet attrs) {
        super(context, attrs, R.attr.serverRecyclerViewStyle);
    }
    public ServerRecyclerView(@NonNull Context context, @Nullable AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
    }
}
