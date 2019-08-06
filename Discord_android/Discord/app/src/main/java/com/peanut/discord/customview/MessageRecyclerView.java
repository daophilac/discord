package com.peanut.discord.customview;

import android.content.Context;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.recyclerview.widget.RecyclerView;
import android.util.AttributeSet;

import com.peanut.discord.R;

public class MessageRecyclerView extends RecyclerView {
    public MessageRecyclerView(@NonNull Context context) {
        super(context);
    }

    public MessageRecyclerView(@NonNull Context context, @Nullable AttributeSet attrs) {
        super(context, attrs, R.attr.messageRecyclerViewStyle);
    }

    public MessageRecyclerView(@NonNull Context context, @Nullable AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
    }
}
